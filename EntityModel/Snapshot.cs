﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EntityModel
{
    public class Snapshot
    {
        [Key]
        public Guid Id { get; set; }

        public bool IsComplete { get; set; }

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public DateTime Date { get; set; }
        public int BedsOpen { get; set; }
        public int BedsWaitlist { get; set; }

        public Snapshot(Guid organizationId)
        {
            this.Id = Guid.NewGuid();
            this.OrganizationId = organizationId;
            this.Date = DateTime.UtcNow;
        }
    }
}
