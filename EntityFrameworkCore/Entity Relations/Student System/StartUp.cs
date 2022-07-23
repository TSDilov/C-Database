
using P01_StudentSystem.Data;
using System;

namespace P01_StudentSystem 
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            var context = new StudentSystemContext();
            context.Database.EnsureCreated();
        }
    }
}
