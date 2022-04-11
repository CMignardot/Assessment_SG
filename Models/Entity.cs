using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ex1.Models
{
    public class Function
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FunctionId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Formula { get; set; }

        public string InputDomains { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string OutputDomain { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string CoDomain { get; set; }

        public Function() { }
        public Function(string name)
        {
            Name = name;
            Formula = "";
            InputDomains = "";
            OutputDomain = "A";
            CoDomain = "A";
        }

        public Function(string name, string formula, string inputDomains, string outputDomain, string codomain)
        {
            Name = name;
            Formula = formula;
            InputDomains = inputDomains;
            OutputDomain = outputDomain;
            CoDomain = codomain;
        }
    }

    public class Constant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConstantId { get; set; }

        [Required]
        public string Name { get; set; }

        public double Value { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string Domain { get; set; }

        public Constant() { }
        public Constant(string name, double value)
        {
            Name = name;
            Value = value;
            Domain = "A";
        }
    }

    public class Expression
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpressionId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Inputs { get; set; }

        public int FunctionId { get; set; } = -1;
        public int ConstantId { get; set; } = -1;

        public Expression() { }
        public Expression(string name, string inputs)
        {
            Name = name;
            Inputs = inputs;
        }

        public Expression(string name, string inputs, int f)
        {
            Name = name;
            Inputs = inputs;
            FunctionId = f;
            ConstantId = -1;
        }

        public Expression(string name, int c)
        {
            Name = name;
            Inputs = "";
            FunctionId = -1;
            ConstantId = c;
        }
    }
}