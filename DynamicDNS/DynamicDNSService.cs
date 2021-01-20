using DynamicDNS.Common;
using DynamicDNS.Extensions;
using DynamicDNS.Models.Cloudflare;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace DynamicDNS
{
    public partial class DynamicDNSService : ServiceBase
    {
        private int eventId = 1;
        private const string EventLogName = "Pandora Application Event";
        private const string EventSourceName = "Dynamic DNS Agent";

        public DynamicDNSService()
        {
            InitializeComponent();
            serviceEventLog = new EventLog();
            if (!EventLog.SourceExists(EventSourceName))
            {
                EventLog.CreateEventSource(EventSourceName, EventLogName);
            }
            serviceEventLog.Source = EventSourceName;
            serviceEventLog.Log = EventLogName;
        }

        private static async Task<string> GetPublicIpv4Address()
        {
            var httpClient = new HttpClient();
            return await httpClient.GetPublicIpv4Address();
        }

        private static async Task<IEnumerable<Zone>> GetZoneList()
        {
            var requestPath = $"zones";
            var httpClient = new HttpClient();

            return await httpClient.GetCloudflare<ICollection<Zone>>(requestPath)
                   ?? new Collection<Zone>();
        }

        private static async Task<IEnumerable<DnsRecord>> GetDnsRecordList(Zone zone)
        {
            var requestPath = $"zones/{zone.Id}/dns_records";
            var httpClient = new HttpClient();

            return await httpClient.GetCloudflare<ICollection<DnsRecord>>(requestPath)
                   ?? new Collection<DnsRecord>();
        }

        private static async Task<DnsRecord> PatchDnsRecord(Zone zone, DnsRecord dnsRecord)
        {
            var requestPath = $"zones/{zone.Id}/dns_records/{dnsRecord.Id}";
            var httpClient = new HttpClient();

            return await httpClient.PatchCloudflare<DnsRecord>(requestPath, dnsRecord);
        }

        private async Task HandleDnsUpdate()
        {
            try
            {
                var recordName = Configuration.GetCloudflareRecordName();

                var zoneList = await GetZoneList();
                var zone = zoneList.FirstOrDefault();
                if (zone is null)
                {
                    throw new Exception("You have no Zone in Cloudflare.");
                }

                serviceEventLog.WriteEntry($"Configuring DNS Record for [Zone: {zone.Name}]",
                    EventLogEntryType.Information, eventId++);

                var dnsRecordList = await GetDnsRecordList(zone);
                var dnsRecord = dnsRecordList.FirstOrDefault(record => record.Name == recordName);
                if (dnsRecord is null)
                {
                    serviceEventLog.WriteEntry($"[Zone: {zone.Name}] No DNS Record found for {recordName}.",
                        EventLogEntryType.Warning, eventId++);
                    return;
                }

                var publicIpv4Address = await GetPublicIpv4Address();
                serviceEventLog.WriteEntry($"[Zone: {zone.Name}] Updating DNS Record of {recordName} to {publicIpv4Address}.",
                    EventLogEntryType.Information, eventId++);

                dnsRecord.Content = publicIpv4Address;
                var patchedDnsRecord = await PatchDnsRecord(zone, dnsRecord);
                if (patchedDnsRecord is null)
                {
                    throw new Exception($"[Zone: {zone.Name}] Invalid response received from CloudFlare API.");
                }

                serviceEventLog.WriteEntry($"[Zone: {zone.Name}] New configuration applied for DNS Record {dnsRecord.Name}.",
                    EventLogEntryType.Information, eventId++);
            }
            catch (Exception e)
            {
                serviceEventLog.WriteEntry(e.Message, EventLogEntryType.Error, eventId++);
            }
        }

        protected override void OnStart(string[] args)
        {
            serviceEventLog.WriteEntry("Dynamic DNS Agent Service started.");
            Task.Run(HandleDnsUpdate);
            var timer = new Timer
            {
                Interval = 60 * 60 * 1000 
            };
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            serviceEventLog.WriteEntry("Dynamic DNS Agent Service stoped.");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            serviceEventLog.WriteEntry("DNS Update Triggered", EventLogEntryType.Information, eventId++);
            Task.Run(HandleDnsUpdate);
        }
    }
}
