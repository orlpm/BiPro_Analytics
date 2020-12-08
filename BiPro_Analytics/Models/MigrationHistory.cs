using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiPro_Analytics.Models
{
    public partial class MigrationHistory
    {
        [Key]
        public string MigrationId { get; set; }
        public string ContextKey { get; set; }
        public byte[] Model { get; set; }
        public string ProductVersion { get; set; }
    }
}