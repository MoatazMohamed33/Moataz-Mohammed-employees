using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class FileService : IFileService
    {
        private List<GroupedProject> _groupbyProject;
        public string getMostPaired(string[] lines)
        {
            List<Employee> employees = extractEmpData(lines);
            _groupbyProject = employees.GroupBy(p => p.ProjectID, (key, g) => new GroupedProject
            {
                ProjectID = key,
                Emps = g.ToList(),
                OverLappedDays = getOverLappedDays(g.First().DateFrom, g.First().DateTo, g.Last().DateFrom, g.Last().DateTo)
            }).OrderByDescending(x => x.OverLappedDays).ToList();
            GroupedProject mostPaired = _groupbyProject.First();
            return ($"EmpID : {mostPaired.Emps.First().EmpID}, and EmpID : {mostPaired.Emps.Last().EmpID} " +
                $"are the most paired for project ID : {mostPaired.ProjectID} with : {mostPaired.OverLappedDays} Days");
        }

        public List<GroupedProject> getData(string[] lines)
        {
            List<Employee> employees = extractEmpData(lines);
            _groupbyProject = employees.GroupBy(p => p.ProjectID, (key, g) => new GroupedProject
            {
                ProjectID = key,
                Emps = g.ToList(),
                OverLappedDays = getOverLappedDays(g.First().DateFrom, g.First().DateTo, g.Last().DateFrom, g.Last().DateTo)
            }).OrderByDescending(x => x.OverLappedDays).ToList();
            return _groupbyProject;
        }

        private List<Employee> extractEmpData(string[] lines)
        {
            DataTable employeesDataTable = ConvertToDataTable(lines);
            List<Employee> employees = new List<Employee>();
            employees = ConvertDataTable<Employee>(employeesDataTable);
            return employees;
        }

        private  DataTable ConvertToDataTable(string[] lines)
        {
            DataTable tbl = new DataTable();
            string firstLine = lines.First();
            lines = lines.Where(l => l != firstLine).ToArray();
            string[] tblColumns = firstLine.Split(",");
            foreach (string col in tblColumns)
            {
                if (col.Contains("ID"))
                {
                    tbl.Columns.Add(new DataColumn(col, typeof(int)));
                }

                if (col.Contains("Date"))
                {
                    tbl.Columns.Add(new DataColumn(col, typeof(DateTime)));
                }
            }

            foreach (string line in lines)
            {
                var cols = line.Split(",");

                DataRow dr = tbl.NewRow();
                for (int cIndex = 0; cIndex < tblColumns.Count(); cIndex++)
                {
                    var colValue = cols[cIndex].Trim().ToLower() == "null" ? DateTime.Now.ToString() : cols[cIndex].Trim();
                    dr[cIndex] = colValue;
                }

                tbl.Rows.Add(dr);
            }

            return tbl;
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        private  double getOverLappedDays(DateTime startFirstPeriod, DateTime endFirstPeriod, DateTime startSecondPeriod, DateTime endSecondPeriod)
        {
            double overLappedDays = 0;
            DateTime maxStart = new[] { startFirstPeriod, startSecondPeriod }.Max();
            DateTime minEnd = new[] { endFirstPeriod, endSecondPeriod }.Min();
            if (minEnd < maxStart)
            {
                return overLappedDays;
            }
            overLappedDays = ((minEnd - maxStart).TotalDays) + 1;
            return overLappedDays;
        }

    }
}
