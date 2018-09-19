using Monq.Tools.MvcExtensions.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monq.Tools.TestExtensions.Tests.TestModels
{
    class TestFilterViewModel
    {
        [FilteredBy(nameof(ValueViewModel.Id))]
        public IEnumerable<int> Ids { get; set; } = null;

        [FilteredBy(nameof(ValueViewModel.Name))]
        public IEnumerable<string> Names { get; set; } = null;

        [FilteredBy(nameof(ValueViewModel.Id))]
        [FilteredBy(nameof(ValueViewModel.Capacity))]
        public IEnumerable<int> IdCaps { get; set; } = null;

        [FilteredBy(nameof(ValueViewModel.Enabled))]
        public bool? Enabled { get; set; } = null;

        [FilteredBy(nameof(ValueViewModel.Name))]
        public string Name { get; set; } = null;
    }

    public class BadFilterModel
    {
        [FilteredBy(nameof(ValueViewModel.Id))]
        public IEnumerable<long> Ids { get; set; }

        [FilteredBy(nameof(ValueViewModel.Name))]
        public IEnumerable<long> Names { get; set; }

        [FilteredBy(nameof(TestFilterViewModel.Names))]
        public IEnumerable<string> Names2 { get; set; }
    }
}