using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Demo
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
    
    
    
    /*
     * in world of classes, we structure by inheritance
     */

    public class Thing
    {
        public Thing(string name)
        {
            
        }
        //...
    }
    
    public class Animal : Thing
    {
        //...
    }

    public class Chinchilla : Animal
    {
        public string Name { get; }
        
        public void Deconstruct(out string s)
        {
            s = "hello!";
        }
        
        public void GoSqueak()
        {
            //...
        }
    }

    public class Rabbit : Animal
    {
        public void Deconstruct(out int i)
        {
            i = 1;
        }
    }
    
    /*
     * and we rely on the C# dispatch rules to distinguish the structure of the called thing
     */
    
    
    /*
     * we can pile on distinctions onto our types via flags, accumulating attributes
     */
    public class Poodle
    {
        public bool IsFluffy { get; }
        
        
        public int Blah(Animal animal) 
        {
            if (animal is Rabbit(13))
            {
                return 4;
            }

            
            if (animal is Chinchilla("Cyril"))
            {
                
            }



            var animal2 = new Chinchilla { Name = "Graham" };

            if (animal is Chinchilla { Name: "Bob" })
            {
                
            }
            
            
            
            switch (animal)
            {
                case Rabbit(var i):
                    Console.WriteLine(i);
                    return 1;
                
                case Chinchilla(var s):
                    Console.WriteLine(s);
                    return 2;
                
                default:
                    return 6;
            }
        }
    }
    
    
    /*
     * with pattern matching, we can split classes apart - FluffyDog vs ShortHairedDog
     * though isn't this inheritance? it is if the splits are essential to the identities of things and further distinctions become increasingly costly
     */
    
    
}