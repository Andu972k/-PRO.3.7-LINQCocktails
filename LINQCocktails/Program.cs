using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace LINQCocktails
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Create ingredients
            Ingredient ingVodka = new Ingredient("Vodka", 15, 40);
            Ingredient ingRum = new Ingredient("Rum", 15, 40);
            Ingredient ingGin = new Ingredient("Gin", 15, 40);
            Ingredient ingTripleSec = new Ingredient("Triple Sec", 20, 30);
            Ingredient ingCola = new Ingredient("Cola", 1, 0);
            Ingredient ingLimeJuice = new Ingredient("Lime Juice", 2, 0);
            Ingredient ingCranJuice = new Ingredient("Cranberry Juice", 2, 0);
            Ingredient ingGingerBeer = new Ingredient("Ginger Beer", 2, 4);
            Ingredient ingMinWater = new Ingredient("Mineral Water", 1, 0);

            List<Ingredient> ingredients = new List<Ingredient>
            {
                ingVodka,
                ingRum,
                ingGin,
                ingTripleSec,
                ingCola,
                ingLimeJuice,
                ingCranJuice,
                ingGingerBeer,
                ingMinWater
            };
            #endregion

            #region Create cocktails
            Cocktail c1 = new Cocktail("Long Island Ice Tea");
            c1.AddIngredient("Rum", 3);
            c1.AddIngredient("Vodka", 3);
            c1.AddIngredient("Gin", 3);
            c1.AddIngredient("Cola", 9);

            Cocktail c2 = new Cocktail("Moscow Mule");
            c2.AddIngredient("Vodka", 4);
            c2.AddIngredient("Lime Juice", 3);
            c2.AddIngredient("Ginger Beer", 10);

            Cocktail c3 = new Cocktail("Cosmopolitan");
            c3.AddIngredient("Vodka", 4);
            c3.AddIngredient("Triple Sec", 2);
            c3.AddIngredient("Lime Juice", 6);
            c3.AddIngredient("Cranberry Juice", 6);

            Cocktail c4 = new Cocktail("Mojito");
            c4.AddIngredient("Rum", 4);
            c4.AddIngredient("Mineral Water", 10);
            c4.AddIngredient("Lime Juice", 2);

            List<Cocktail> cocktails = new List<Cocktail> { c1, c2, c3, c4 };
            #endregion


            var cocktailName = from c in cocktails
                select c.Name;

            foreach (string s in cocktailName)
            {
                Console.WriteLine(s);
            }

            //foreach (string s in cocktails.Select(c => c.Name))
            //{
            //    Console.WriteLine(s);
            //}

            Console.WriteLine();

            var query2 = from c in cocktails
                select new {c.Name, c.Ingredients};

            foreach (var c in query2)
            {
                Console.WriteLine(c.Name);
                foreach (var s in c.Ingredients)
                {
                    Console.WriteLine($"{s.Key}  {s.Value} amount");
                }
            }

            Console.WriteLine();

            //foreach (var x1 in cocktails.Select(c => new { c.Name, c.Ingredients }))
            //{
            //    Console.WriteLine(x1.Name);
            //    foreach (var ingredient in x1.Ingredients)
            //    {
            //        Console.WriteLine($"{ingredient.Key}  {ingredient.Value} amount");
            //    }

            //}


            Console.WriteLine();


            //var query3 = from c in cocktails
            //    where (from ingredient in ingredients
            //        where ingredient.AlcoholPercent > 10
            //        select ingredient.AlcoholPercent)
            //    select new {c.Name, c.Ingredients};

            //var query3 = from c in cocktails
            //    join i in ingredients on c.Ingredients.Keys equals i.Name
            //    where i.AlcoholPercent > 10
            //    select new {c.Name };

            var query3 = from c in cocktails
                select new
                {
                    c.Name, ing = (from cocktailing in c.Ingredients
                        join ingredient in ingredients on cocktailing.Key equals ingredient.Name
                        where ingredient.AlcoholPercent > 10
                        select ingredient.Name)
                };

            foreach (var q in query3)
            {
                Console.WriteLine($"The Cocktail {q.Name}");
                foreach (string s in q.ing)
                {
                    Console.WriteLine($"The ingredient {s} has an alcoholpercentage over 10");
                }
            }

            Console.WriteLine();



            //var query4 = from c in cocktails
            //    select new
            //    {
            //        c.Name, Price = (from i in c.Ingredients
            //            join ing in ingredients on i.Key equals ing.Name
            //            select new {amount = i.Value, pricePrCL = ing.PricePerCl})
            //    };

            //.Aggregate(1, (int v, var p) => {v*p})



            //foreach (var q in query4)
            //{
            //    Console.WriteLine($"Cocktail {q.Name}");



            //    int price = 0;

            //    foreach (var p in q.Price)
            //    {
            //        price = price + (p.amount * p.pricePrCL);
            //    }

            //    Console.WriteLine($"Drink price {price}");
            //}

            var query4 = from c in cocktails
                select new
                {
                    c.Name,
                    Price = (from i in c.Ingredients
                        join ing in ingredients on i.Key equals ing.Name
                        select i.Value * ing.PricePerCl).Sum()
                };

            foreach (var q in query4)
            {
                Console.WriteLine($"Cocktail {q.Name} costs {q.Price}");
            }

            Console.WriteLine();



            var query5 = from c in cocktails
                select new
                {
                    c.Name, AlcoholPercentage = ((from i in c.Ingredients
                        join ing in ingredients on i.Key equals ing.Name
                        select ((i.Value * (ing.AlcoholPercent / 100)))).Sum()/((from i2 in c.Ingredients
                                                    select i2.Value).Sum()))*100
                };

            foreach (var q in query5)
            {
                Console.WriteLine($"CocktailName {q.Name} contains {q.AlcoholPercentage}");
            }

            Console.WriteLine();


            KeepConsoleWindowOpen();
        }

        private static void KeepConsoleWindowOpen()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to close application");
            Console.ReadKey();
        }
    }
}
