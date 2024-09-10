using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
    public static class ExportHelper
    {
        public static string ConvertToCsv<T>(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties();
            var csvBuilder = new StringBuilder();

            // Add header
            csvBuilder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Add rows
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? string.Empty);
                csvBuilder.AppendLine(string.Join(",", values));
            }

            return csvBuilder.ToString();
        }
    }
}
