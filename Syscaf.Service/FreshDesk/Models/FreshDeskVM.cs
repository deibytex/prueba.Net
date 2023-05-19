using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.FreshDesk.Models
{
    public class FreshDeskVM
    {
        public string id { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        public int position { get; set; }
        public bool required_for_closure { get; set; }
        public bool required_for_agents { get; set; }
        public string type { get; set; }
        [Display(Name = "default")]
        public bool  defecto { get; set; }
        public bool customers_can_edit { get; set; }
        public bool customers_can_filter { get; set; }
        public string label_for_customers { get; set; }
        public bool required_for_customers { get; set; }
        public bool displayed_to_customers { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool portal_cc { get; set; }
        public string portal_cc_to { get; set; }
        public List<object> choices { get; set; }
    }
}
