using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ex1.Models
{
    public class EFCs
    {
        public int ExpressionId { get; set; } = 0;
        public Expression Expression { get; set; }
        public List<Expression> Expressions { get; set; }
        public List<Function> Functions { get; set; }
        public List<Constant> Constants { get; set; }

        public EFCs(int id, List<Expression> expressions, List<Function> functions, List<Constant> constants)
        {
            ExpressionId = id;
            Expressions = expressions;
            Functions = functions;
            Constants = constants;
        }

        public EFCs(List<Expression> expressions, List<Function> functions, List<Constant> constants)
        {
            Expressions = expressions;
            Functions = functions;
            Constants = constants;
        }
    }

    public class EvaluateExpression
    {
        public string Evaluation { get; set; } = string.Empty;
    }

    public class AddExpressionModel
    {
        public Expression Expression { get; set; }
        public SelectList Functions { get; set; }
        public SelectList Constants { get; set; }

        public int FunctionId { get; set; } = -1;
        public int ConstantId { get; set; } = -1;

        public AddExpressionModel() { }
        public AddExpressionModel(DBContext dB, int id)
        {
            var functions = from f in dB.Functions orderby f.Name select f;
            var constants = from c in dB.Constants orderby c.Name select c;

            Expression = dB.Expressions.Find(id);

            object selectedFunction = functions.FirstOrDefault(f => f.FunctionId == 0);
            object selectedConstant = constants.FirstOrDefault(c => c.ConstantId == 0);

            Functions = new SelectList(functions.AsNoTracking(), "FunctionId", "Name", selectedFunction);
            Constants = new SelectList(constants.AsNoTracking(), "ConstantId", "Name", selectedConstant);
        }

        public AddExpressionModel(DBContext dB)
        {
            var functions = from f in dB.Functions orderby f.Name select f;
            var constants = from c in dB.Constants orderby c.Name select c;

            object selectedFunction = functions.FirstOrDefault(f => f.FunctionId == 0);
            object selectedConstant = constants.FirstOrDefault(c => c.ConstantId == 0);

            Functions = new SelectList(functions.AsNoTracking(), "FunctionId", "Name", selectedFunction);
            Constants = new SelectList(constants.AsNoTracking(), "ConstantId", "Name", selectedConstant);
        }
    }
}
