using points.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace points.ModelViews.Reports
{
    public class CycleReportModelView
    {

        public TimesOfEvaluationAndPerformance Cyckle { get; set; }
        public IEnumerable<Employee> Employees { get; set; }

    }
}
