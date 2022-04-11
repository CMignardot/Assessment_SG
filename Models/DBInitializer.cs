using System;
using System.Linq;

namespace Ex1.Models
{
    public static class DBInitializer
    {
        public static void Initialize(DBContext db)
        {
            db.Database.EnsureCreated();

            if (db.Expressions != null) db.RemoveRange(entities: db.Expressions!);
            db.SaveChanges();

            if (db.Functions != null) db.RemoveRange(entities: db.Functions!);
            if (db.Constants != null) db.RemoveRange(entities: db.Constants!);
            db.SaveChanges();

            var f1 = db.Functions?.Add(new("f1")).Entity;
            var f2 = db.Functions?.Add(new("f2")).Entity;
            var f3 = db.Functions?.Add(new("f3")).Entity;
            var f4 = db.Functions?.Add(new("f4")).Entity;
            var c1 = db.Constants?.Add(new("c1", 1)).Entity;
            var c2 = db.Constants?.Add(new("c2", 2)).Entity;
            var c3 = db.Constants?.Add(new("c3", 3)).Entity;
            var c4 = db.Constants?.Add(new("c4", 4)).Entity;
            var c5 = db.Constants?.Add(new("c5", 5)).Entity;
            db.SaveChanges();

            var exp1 = db.Expressions?.Add(new("f1i", "c1i,f2i,f3i", f1.FunctionId)).Entity;
            var exp2 = db.Expressions?.Add(new("f2i", "c2i,c3i", f2.FunctionId)).Entity;
            var exp3 = db.Expressions?.Add(new("f3i", "f4i", f3.FunctionId)).Entity;
            var exp4 = db.Expressions?.Add(new("f4i", "c5i,c5i", f4.FunctionId)).Entity;
            var exp5 = db.Expressions?.Add(new("c1i", c1.ConstantId)).Entity;
            var exp6 = db.Expressions?.Add(new("c2i", c2.ConstantId)).Entity;
            var exp7 = db.Expressions?.Add(new("c3i", c3.ConstantId)).Entity;
            var exp8 = db.Expressions?.Add(new("c4i", c4.ConstantId)).Entity;
            var exp9 = db.Expressions?.Add(new("c5i", c5.ConstantId)).Entity;
            db.SaveChanges();

            var g1 = db.Functions?.Add(new("g1")).Entity;
            var g2 = db.Functions?.Add(new("g2")).Entity;
            var d1 = db.Constants?.Add(new("d1", 1)).Entity;
            var d2 = db.Constants?.Add(new("d2", 2)).Entity;
            db.SaveChanges();

            var exp10 = db.Expressions?.Add(new("g1a", "g1b,g2a", g1.FunctionId)).Entity;
            var exp20 = db.Expressions?.Add(new("g1b", "d1a,d2a", g1.FunctionId)).Entity;
            var exp30 = db.Expressions?.Add(new("g2a", "d2a,d2a", g2.FunctionId)).Entity;
            var exp40 = db.Expressions?.Add(new("d1a", d1.ConstantId)).Entity;
            var exp50 = db.Expressions?.Add(new("d2a", d2.ConstantId)).Entity;
            db.SaveChanges();
        }
    }
}