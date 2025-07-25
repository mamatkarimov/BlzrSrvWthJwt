
using System;
using System.Collections.Generic;
namespace Backend.API.Domain.Entities
{
    public class TestTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ICollection<AssignedTest> AssignedTests { get; set; } = new List<AssignedTest>();
        //public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }

}