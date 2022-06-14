using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(c => c.Orders.Sum(o => o.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.Select(c =>
                (c, suppliers.Where(s => s.Country == c.Country && s.City == c.City)
                ));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
                    IEnumerable<Customer> customers,
                    IEnumerable<Supplier> suppliers
                )
        {
            return customers.GroupBy(
                c => c,
                c => suppliers.Where(s => s.Country == c.Country && s.City == c.City),
                (c, s) => (c, s.First()));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            // I think the task for Linq3 is the same as for Linq1 but the unit test for Linq3 is wrong.
            return customers.Where(c => (decimal)c.Orders.Sum(o => o.Total) > limit);
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            return customers
                    .Where(c => c.Orders.Any())
                    .Select(c => (c, c.Orders.Select(o => o.OrderDate).Min()));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            return customers
                    .Where(c => c.Orders.Any())
                    .Select(c => (c, c.Orders.Select(o => o.OrderDate).Min()))
                    .OrderBy(c => c.Item2.Year)
                    .ThenBy(c => c.Item2.Month)
                    .ThenByDescending(c => c.Item1.Orders.Select(o => o.Total).Sum())
                    .ThenBy(c => c.Item1.CompanyName);

        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            return customers.Where(c =>
                c.PostalCode.Except("0123456789").Any()
                || String.IsNullOrEmpty(c.Region)
                || !Regex.IsMatch(c.Phone, @"\(\d*\)"));
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            /* example of Linq7result

             category - Beverages
	            UnitsInStock - 39
		            price - 18.0000
		            price - 19.0000
	            UnitsInStock - 17
		            price - 18.0000
		            price - 19.0000
             */

            return products.GroupBy(
                p => p.Category,
                (category, p) => new Linq7CategoryGroup()
                {
                    Category = category,
                    UnitsInStockGroup = p.Where(p => p.Category == category)
                        .GroupBy(
                            p => p.UnitsInStock,
                            (qty, p) => new Linq7UnitsInStockGroup()
                            {
                                UnitsInStock = qty,
                                Prices = p.Where(p => p.UnitsInStock == qty).Select(p => p.UnitPrice).OrderBy(p => p)
                            })
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            return products.GroupBy(
                p =>
                {
                    if (p.UnitPrice <= cheap)
                    {
                        return cheap;
                    }
                    else if (p.UnitPrice <= middle)
                    {
                        return middle;
                    }
                    else
                    {
                        return expensive;
                    };
                },
                (category, p) => (category, p)
                );
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            return customers.GroupBy(
                c => c.City,
                c => c, // customer list
                (city, customer_list) => (city,
                                          (int)Math.Round(customer_list.SelectMany(c => c.Orders).Select(o => o.Total).Sum() / customer_list.Count()),
                                          (int)Math.Round((decimal)customer_list.Select(c => c.Orders.Count()).Sum() / customer_list.Count())
                                          )
                );
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            return String.Concat(suppliers.Select(s => s.Country).Distinct()
                .GroupBy(name => name.Length, 
                         name => name, 
                         (length, names) => new { Length = length, 
                                                  Countries = names.OrderBy(n => n) }
                         )
                .OrderBy(kvp => kvp.Length).SelectMany(kvp => kvp.Countries)
                );
        }
    }
}