using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ex1.Models;
using System.Collections.Generic;

namespace Ex1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DBContext ctx = new();

        public HomeController(DBContext context) => ctx = context;

        private string WriteExpression(Expression exp)
        {
            Console.WriteLine($"ici - {exp.FunctionId != -1} - {exp.ConstantId != -1}");
            if (exp.FunctionId != -1)
            {
                var inputs = exp.Inputs.Split(',');
                List<string> s = new();
                foreach (string input in inputs)
                {
                    var exp2 = ctx.Expressions?.First(e => e.Name == input);
                    if (exp2 != null) s.Add(WriteExpression(exp2));
                }
                string fname = ctx.Functions.Find(exp.FunctionId).Name;
                return fname + "(" + s.Aggregate((a, b) => a + ", " + b) + ")";
            }

            else if (exp.ConstantId != -1) return ctx.Constants.Find(exp.ConstantId).Name;

            else return string.Empty;
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        public IActionResult Index() => View(new EFCs(ctx.Expressions.ToList(), ctx.Functions.ToList(), ctx.Constants.ToList()));
        public IActionResult ShowConstant() => View(ctx.Constants.ToList());
        public IActionResult ShowFunction() => View(ctx.Functions.ToList());

        public IActionResult AddNewExpression() => View(new AddExpressionModel(ctx));
        public IActionResult AddNewConstant() => View();
        public IActionResult AddNewFunction() => View();

        public IActionResult ModifyExpression(int id) => View(new AddExpressionModel(ctx, id));
        public IActionResult ModifyConstant(int id) => View(ctx.Constants.FirstOrDefault(e => e.ConstantId == id));
        public IActionResult ModifyFunction(int id) => View(ctx.Functions.FirstOrDefault(e => e.FunctionId == id));

        public IActionResult EvaluateExpression(int id)
        {
            var e = ctx.Expressions.Find(id);
            EvaluateExpression eval = new();
            if (e != null) eval.Evaluation = WriteExpression(e);
            Console.WriteLine($"exp: {eval.Evaluation}");
            return View(eval);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewExpression(AddExpressionModel model)
        {
            Expression exp = model.Expression;
            if (exp != null)
            {
                var f = ctx.Functions.Find(model.FunctionId);
                var c = ctx.Constants.Find(model.ConstantId);
                if (f != null) ctx.Expressions.Add(new(exp.Name, exp.Inputs, f.FunctionId));
                else if (c != null) ctx.Expressions.Add(new(exp.Name, c.ConstantId));
                else ctx.Expressions.Add(new(exp.Name, exp.Inputs));
                await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewConstant(Constant e)
        {
            if (ModelState.IsValid)
            {
                ctx.Constants.Add(e);
                await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(ShowConstant));
            }
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewFunction(Function e)
        {
            if (ModelState.IsValid)
            {
                ctx.Functions.Add(e);
                await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(ShowFunction));
            }
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> ModifyExpression(AddExpressionModel model)
        {
            Expression exp = model.Expression;
            Expression e = ctx.Expressions.FirstOrDefault(e => e.ExpressionId == exp.ExpressionId);
            if (e != null)
            {
                e.Name = exp.Name;
                e.Inputs = exp.Inputs;
                if (ctx.Functions.Find(model.FunctionId) != null)
                {
                    e.FunctionId = model.FunctionId;
                    e.ConstantId = -1;
                    ctx.Expressions.Update(e);
                }
                else if (ctx.Constants.Find(model.ConstantId) != null)
                {
                    e.FunctionId = -1;
                    e.ConstantId = model.ConstantId;
                    ctx.Expressions.Update(e);
                }
                else
                {
                    e.FunctionId = -1;
                    e.ConstantId = -1;
                    ctx.Expressions.Update(e);
                }
                await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyConstant(Constant c)
        {
            if (ModelState.IsValid)
            {
                Constant e = ctx.Constants.FirstOrDefault(e => e.ConstantId == c.ConstantId);
                if (e != null)
                {
                    e.Name = c.Name;
                    e.Value = c.Value;
                    e.Domain = c.Domain;
                    ctx.Constants.Update(e);
                    await ctx.SaveChangesAsync();
                }
                return RedirectToAction(nameof(ShowConstant));
            }
            else
                return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyFunction(Function f)
        {
            if (ModelState.IsValid)
            {
                Function e = ctx.Functions.FirstOrDefault(e => e.FunctionId == f.FunctionId);
                if (e != null)
                {
                    e.Name = f.Name;
                    e.Formula = f.Formula;
                    e.InputDomains = f.InputDomains;
                    e.OutputDomain = f.OutputDomain;
                    e.CoDomain = f.CoDomain;
                    ctx.Functions.Update(e);
                    await ctx.SaveChangesAsync();
                }
                return RedirectToAction(nameof(ShowFunction));
            }
            else
                return View(f);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveExpression(int id)
        {
            try
            {
                var del = ctx.Expressions.Where(e => e.ExpressionId == id).FirstOrDefault();
                ctx.Expressions.Remove(del);
                await ctx.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConstant(int id)
        {
            try
            {
                var del = ctx.Constants.Where(e => e.ConstantId == id).FirstOrDefault();
                ctx.Constants.Remove(del);
                await ctx.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(ShowConstant));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFunction(int id)
        {
            try
            {
                var del = ctx.Functions.Where(e => e.FunctionId == id).FirstOrDefault();
                ctx.Functions.Remove(del);
                await ctx.SaveChangesAsync();
            }
            catch { }
            return RedirectToAction(nameof(ShowFunction));
        }
    }
}