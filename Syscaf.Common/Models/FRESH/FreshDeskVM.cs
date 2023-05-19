using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.FRESH
{
    public class TicketsFreshDesk
    {
        public string[] cc_emails { get; set; }
        public string[] fwd_emails { get; set; }
        public string[] reply_cc_emails { get; set; }
        public string[] ticket_cc_emails { get; set; }
        public bool fr_escalated { get; set; }
        public bool spam { get; set; }
        public string email_config_id { get; set; }
        public int? group_id { get; set; }
        public int priority { get; set; }
        public string requester_id { get; set; }
        public string responder_id { get; set; }
        public int source { get; set; }
        public int? company_id { get; set; }
        public int status { get; set; }
        public string subject { get; set; }
        public string association_type { get; set; }
        public int MyProperty { get; set; }
        public string support_email { get; set; }
        public dynamic to_emails { get; set; }
        public int? product_id { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public DateTime due_by { get; set; }
        public DateTime fr_due_by { get; set; }
        public bool is_escalated { get; set; }
        public dynamic custom_fields { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string associated_tickets_count { get; set; }
        public string[] tags { get; set; }
    }
    public class FreshDeskVM
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        public int position { get; set; }
        public bool required_for_closure { get; set; }
        public bool required_for_agents { get; set; }
        public string type { get; set; }
        [Display(Name = "default")]
        public bool defecto { get; set; }
        public bool customers_can_edit { get; set; }
        public bool customers_can_filter { get; set; }
        public string label_for_customers { get; set; }
        public bool required_for_customers { get; set; }
        public bool displayed_to_customers { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool portal_cc { get; set; }
        public string portal_cc_to { get; set; }
       // public List<dynamic[]> choices { get; set; }
    }
}
