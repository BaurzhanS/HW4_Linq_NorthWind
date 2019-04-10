using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_LinqQueries
{
    public class NorthwindDbContextTraining
    {
        public void Task_1()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var customerInfo = dbContext.Customers
                .ToList()
                .Select(p => new
                {
                    CompanyName = p.CompanyName,
                    Address = $"{p.Country}, {p.City}, {p.Address}"
                });

            var customerInfo2 = from p in dbContext.Customers.ToList()
                                select new
                                {
                                    CompanyName = p.CompanyName,
                                    Address = $"{p.Country}, {p.City}, {p.Address}"
                                };


            foreach (var item in customerInfo)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            foreach (var item in customerInfo2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_2()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var OrderInfo = dbContext.Orders
                .Where(p => p.ShippedDate.HasValue)
                .ToList().Select(p => new
                {
                    OrderId = p.OrderID,
                    Address = $"{p.ShipCountry}, {p.ShipCity}, {p.ShipAddress}",
                    CountDays = (p.ShippedDate - p.OrderDate).Value.Days
                });
            var OrderInfo2 = from p in dbContext.Orders.ToList()
                             where p.ShippedDate.HasValue
                             select new
                             {
                                 OrderId = p.OrderID,
                                 Address = $"{p.ShipCountry}, {p.ShipCity}, {p.ShipAddress}",
                                 CountDays = (p.ShippedDate - p.OrderDate).Value.Days
                             };
            foreach (var item in OrderInfo)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            foreach (var item in OrderInfo2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_3()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var ProductInfo = dbContext.Products
                .ToList().Select(p => new
                {
                    ProductName = p.ProductName,
                    ProductTotalCost = String.Format("{0:C}", (p.UnitsInStock * p.UnitPrice))
                });

            var ProductInfo2 = from p in dbContext.Products.ToList()
                               select new
                               {
                                   ProductName = p.ProductName,
                                   ProductTotalCost = p.UnitsInStock * p.UnitPrice
                               };
            foreach (var item in ProductInfo)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //foreach (var item in ProductInfo2)
            //{
            //    Console.WriteLine(item);
            //}
        }

        public void Task_4()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var EmployeesInUSA = dbContext.Employees
                .Where(w => w.Country == "USA")
                .ToList().Select(p => new
                {
                    EmployeeFName = p.FirstName,
                    EmployeeLName = p.LastName
                });

            foreach (var item in EmployeesInUSA)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var EmployeeInUSA2 = from p in dbContext.Employees.ToList()
                                 where p.Country == "USA"
                                 select p;

            var EmployeeInUSA22 = from p in dbContext.Employees.ToList()
                                  where p.Country == "USA"
                                  select new { p.FirstName, p.Address };

            foreach (var item in EmployeeInUSA2)
            {
                Console.WriteLine(item);
            }

            foreach (var item in EmployeeInUSA22)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_5()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var EmployeesInUSA = dbContext.Employees
                .ToList()
                .Where(w => (DateTime.Now - w.BirthDate).Value.Days / 365 > 50)
                .Select(p => new
                {
                    EmployeeFName = p.FirstName,
                    EmployeeLName = p.LastName,
                    EmployeeAge = (DateTime.Now - p.BirthDate).Value.Days / 365
                });

            foreach (var item in EmployeesInUSA)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var EmployeeInUSA2 = from p in dbContext.Employees.ToList()
                                 where (DateTime.Now - p.BirthDate).Value.Days / 365 > 50
                                 select p;

            var EmployeeInUSA22 = from p in dbContext.Employees.ToList()
                                  where (DateTime.Now - p.BirthDate).Value.Days / 365 > 50
                                  select new { p.FirstName, p.Address };

            foreach (var item in EmployeeInUSA2)
            {
                Console.WriteLine(item);
            }

            foreach (var item in EmployeeInUSA22)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_6()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var employeesBelgium = dbContext.Employees
                .Where(p => p.Orders.FirstOrDefault(x => x.ShipCountry == "Belgium") != null).Select(p => new
                {
                    p.EmployeeID,
                    p.LastName,
                });


            foreach (var item in employeesBelgium)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var employees = from e in dbContext.Employees
                                    join o in dbContext.Orders on
                                    e.EmployeeID equals o.EmployeeID
                                    where o.ShipCountry == "Belgium"
                                    select new
                                    {
                                        e.FirstName,
                                        e.LastName
                                    };
            var employeesBelgium2 = from emp in employees group emp by emp.LastName;
            foreach (var item in employeesBelgium2)
            {
                Console.WriteLine(item.Key);
            }
        }

        public void Task_14()
        {
            NorthwindEntities dbContext = new NorthwindEntities();

            var complaintManagerInfoPerOrder1 =
                from order in dbContext.Orders
                select new
                {
                    OrderId = order.OrderID,
                    ComplaintManagerId = (
                        from emp in dbContext.Employees
                        where emp.EmployeeID == order.EmployeeID
                        select emp)
                        .FirstOrDefault().ReportsTo
                };

            var complaintManagerInfoPerOrder2 =
                from order in dbContext.Orders
                join employee in dbContext.Employees
                on order.EmployeeID equals employee.EmployeeID
                select new { order.OrderID, employee.ReportsTo };

            var complaintManagerInfoPerOrder3 =
                dbContext.Orders.Select(p => new
                {
                    OrderId = p.OrderID,
                    ComplaintManagerId =
                        p.Employees.ReportsTo.HasValue ?
                        p.Employees.Employees2.EmployeeID : p.EmployeeID
                });

            foreach (var item in complaintManagerInfoPerOrder3)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_7()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            //var productsId = dbContext.Order_Details.Select(p => new
            //{
            //    ProductId = p.ProductID,
            //    ProductQty = p.Quantity
            //}).GroupBy(p => p.ProductId);

            var PopularProductsSumQty = dbContext.Order_Details.GroupBy(p => p.ProductID).Select(s => new
            {
                ProductId = s.FirstOrDefault().ProductID,
                SumQtyOrdered = s.Sum(x => x.Quantity),
                SumQtyInStock = s.FirstOrDefault().Products.UnitsInStock
            }).Where(x => x.SumQtyOrdered > x.SumQtyInStock);

            var OrderedProductsSumQty2=from o in dbContext.Order_Details
                                       group o by o.ProductID into products
                                       select new
                                       {
                                           ProductId=products.Key,
                                           QtyOrdered=products.Sum(p=>p.Quantity)
                                       };

            var PopularProductsSumQty2 = from p in dbContext.Products
                                         join q in OrderedProductsSumQty2 on
                                         p.ProductID equals q.ProductId
                                         where q.QtyOrdered > p.UnitsInStock
                                         select new
                                         {
                                             Product=p.ProductName,
                                             OrderedQty=q.QtyOrdered,
                                             QtyInStock=p.UnitsInStock
                                         };

            foreach (var item in PopularProductsSumQty2)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

        }

        public void Task_8()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var EmployeesOrders = dbContext.Orders.GroupBy(g => g.EmployeeID).Select(p => new
            {
                Employee = p.FirstOrDefault().Employees.FirstName,
                Orders = p.Count()
            });

            foreach (var item in EmployeesOrders)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var EmployeesOrders2 = from e in dbContext.Employees
                                   join o in dbContext.Orders on
                                   e.EmployeeID equals o.EmployeeID
                                   group e by e.FirstName into orders
                                   select new
                                   {
                                       Employees = orders.Key,
                                       Orders = orders.Count()
                                   };
                            
            
            foreach (var item in EmployeesOrders2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_9()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var OrdersCus = dbContext.Order_Details.GroupBy(g => g.OrderID).Select(p => new
            {
                OrderId = p.FirstOrDefault().OrderID,
                SumOrdered = p.Sum(s => s.UnitPrice * s.Quantity),
                //CustomerId=p.FirstOrDefault().Orders.CustomerID,
                Customer=p.FirstOrDefault().Orders.Customers.CompanyName
            });

            //foreach (var item in OrdersCus)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();

            var OrdersCus2 = from c in dbContext.Customers
                             join o in dbContext.Orders on
                             c.CustomerID equals o.CustomerID
                             join od in dbContext.Order_Details
                             on o.OrderID equals od.OrderID
                             select new
                             {
                                 Customer=c.CompanyName,
                                 OrderId = o.OrderID,
                                 Summa = od.Quantity * od.UnitPrice
                             };
            var customers = from cus in OrdersCus2
                            group cus by cus.Customer into orders
                            select new
                            {
                                Customer=orders.FirstOrDefault().Customer,
                                TotalSum=orders.Sum(p=>p.Summa)

                            };


            foreach (var item in customers)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_10()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var Products = dbContext.Order_Details.GroupBy(g => g.ProductID).Select(p => new
            {
                ProductId = p.FirstOrDefault().ProductID,
                ProductName=p.FirstOrDefault().Products.ProductName,
                ProductQty=p.FirstOrDefault().Quantity,
                SumOrdered = p.Sum(s => s.UnitPrice * s.Quantity)
            });

            //foreach (var item in Products)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();

            var Products2 = from p in dbContext.Products
                            join od in dbContext.Order_Details on
                            p.ProductID equals od.ProductID
                            group new { p, od } by new { p.ProductID, p.ProductName } into products
                            orderby products.Key.ProductID
                            select new
                            {
                                ProductId = products.Key.ProductID,
                                //ProductName=products.Key.ProductName,
                                ProductQty = products.Sum(s => s.od.Quantity),
                                SumOrdered = products.Sum(s => s.od.Quantity * s.od.UnitPrice)
                            };


            foreach (var item in Products2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_11()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var Customers = dbContext.Orders.GroupBy(g=>g.ShipCountry).Select(p => new
            {
                Country=p.Key,
                Count=p.Count()
            });

            //foreach (var item in Customers)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();

            var Customers2 = from o in dbContext.Orders
                            group o by o.ShipCountry into countries
                            
                            select new
                            {
                                Country=countries.Key,
                                Count= countries.Count()
                            };


            foreach (var item in Customers2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_12()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var Customers = dbContext.Orders.GroupBy(g => g.CustomerID).Select(p => new
            {
                //CustomerId = p.Key,
                CustomerName=p.FirstOrDefault().Customers.CompanyName,
                Country = p.FirstOrDefault().ShipCountry
            });

            foreach (var item in Customers)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var Customers2 = from o in dbContext.Orders
                             join c in dbContext.Customers
                             on o.CustomerID equals c.CustomerID
                             group new { o,c } by new { o.CustomerID } into countries

                             select new
                             {
                                 CustomerName = countries.FirstOrDefault().c.CompanyName,
                                 Country = countries.FirstOrDefault().o.ShipCountry
                             };


            foreach (var item in Customers2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_13()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var Customers = dbContext.Order_Details.Select(p => new
            {
                //CustomerId = p.Key,
                CustomerName = p.Orders.Customers.CompanyName,
                SumOrdered = p.Quantity * p.UnitPrice
            }).
            GroupBy(p => p.CustomerName).Select(p => new
            {
                CustomerName = p.Key,
                Minimum = p.Min(m => m.SumOrdered),
                Maximum = p.Max(m => m.SumOrdered)
            }).OrderBy(o => o.CustomerName);

            foreach (var item in Customers)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var Customers2 = from o in dbContext.Orders
                             join c in dbContext.Customers
                             on o.CustomerID equals c.CustomerID
                             join od in dbContext.Order_Details
                             on o.OrderID equals od.OrderID
                             group new { o, c, od } by new { c.CompanyName } into customers
                             orderby customers.Key.CompanyName
                             select new
                             {
                                 CustomerName = customers.Key.CompanyName,
                                 Minimum = customers.Min(m=>m.od.Quantity*m.od.UnitPrice),
                                 Maximum = customers.Max(m => m.od.Quantity * m.od.UnitPrice)
                             };


            foreach (var item in Customers2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_15()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var Customers = dbContext.Orders.GroupBy(p => p.CustomerID).Select(p => new
            {
                //CustomerId = p.Key,
                CustomerName = p.FirstOrDefault().Customers.CompanyName,
                Count = p.Count()
            }).OrderByDescending(p => p.Count).Take(3);
           

            foreach (var item in Customers)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var Customers2 = (from o in dbContext.Orders
                             join c in dbContext.Customers
                             on o.CustomerID equals c.CustomerID
                             group new { o, c } by new { c.CompanyName } into customers
                             orderby customers.Count() descending
                             select new
                             {
                                 CustomerName = customers.Key.CompanyName,
                                 Count=customers.Count()
                             }).Take(3);


            foreach (var item in Customers2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_16()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var Customers = dbContext.Order_Details.Select(p => new
            {
                //CustomerId = p.Key,
                CustomerName = p.Orders.Customers.CompanyName,
                ProductCategory = p.Products.Categories.CategoryName
            }).GroupBy(p=>new
            {
                p.CustomerName,
                p.ProductCategory
            }).Select(p=>new
            {
                CustomerName=p.FirstOrDefault().CustomerName,
                ProductCategory=p.FirstOrDefault().ProductCategory
            }) ;


            //foreach (var item in Customers)
            //{
            //    Console.WriteLine(item);
            //}
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var Customers2 = (from o in dbContext.Orders
                              join od in dbContext.Order_Details
                              on o.OrderID equals od.OrderID
                              join c in dbContext.Customers
                              on o.CustomerID equals c.CustomerID
                              join p in dbContext.Products
                              on od.ProductID equals p.ProductID
                              join cat in dbContext.Categories
                              on p.CategoryID equals cat.CategoryID
                              group new { o,od,c,p,cat } by new { c.CompanyName, cat.CategoryName } into customers
                              orderby customers.FirstOrDefault().c.CompanyName
                              select new
                              {
                                  CustomerName = customers.Key.CompanyName,
                                  ProductCategory = customers.Key.CategoryName
                              });


            foreach (var item in Customers2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_17()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var Customers = dbContext.Orders.Select(p => new
            {
                SupplierName = p.Order_Details.FirstOrDefault().Products.Suppliers.CompanyName,
                SupplierCity = p.Order_Details.FirstOrDefault().Products.Suppliers.City,
                CustomerName = p.Customers.CompanyName,
                CustomerCity = p.Customers.City,
                OrderedCity = p.ShipCity
            }).Where(p => p.SupplierCity == p.CustomerCity);


            foreach (var item in Customers)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var Customers2 = from od in dbContext.Order_Details
                             join p in dbContext.Products
                             on od.ProductID equals p.ProductID
                             join o in dbContext.Orders
                             on od.OrderID equals o.OrderID
                             join c in dbContext.Customers
                             on o.CustomerID equals c.CustomerID
                             join s in dbContext.Suppliers
                             on p.SupplierID equals s.SupplierID
                             where o.ShipCity == s.City
                             select (new
                             {
                                 SupplierName=s.CompanyName,
                                 SupplierCity=s.City,
                                 CustomerName=c.CompanyName,
                                 CustomerCity=c.City,
                                 OrderedCity=o.ShipCity
                             });


            foreach (var item in Customers2)
            {
                Console.WriteLine(item);
            }
        }

        public void Task_18()
        {
            NorthwindEntities dbContext = new NorthwindEntities();
            var Customers = dbContext.Orders.Where(p=>p.Shippers.CompanyName == "Speedy Express" && p.ShipCity== "Bruxelles")
                .Select(p => new
            {
                    p.Employees.FirstName,
                    p.Customers.CompanyName

            });


            foreach (var item in Customers)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var Customers2 = (from o in dbContext.Orders
                              join c in dbContext.Customers
                              on o.CustomerID equals c.CustomerID
                              join e in dbContext.Employees
                              on o.EmployeeID equals e.EmployeeID
                              where o.Shippers.CompanyName == "Speedy Express" && o.ShipCity== "Bruxelles"
                              select new
                              {
                                  e.FirstName,
                                  c.CompanyName
                              });


            foreach (var item in Customers2)
            {
                Console.WriteLine(item);
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            NorthwindDbContextTraining dbContextTraining = new NorthwindDbContextTraining();
            dbContextTraining.Task_18();
            //LinqQuerySyntaxDemo.SelectQueryDemo_II();

            Console.ReadLine();

        }
    }
}
