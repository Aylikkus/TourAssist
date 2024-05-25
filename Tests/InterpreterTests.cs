using System.Text.Json;
using TourAssist.Model;

namespace Tests
{
    [TestFixture]
    public class InterpreterTests
    {
        private BinaryTree moreipalmyAST;
        private BinaryTree moreipalmyililesAST;
        private BinaryTree complexAST;

        public static void EqualByJSON(object object1, object object2)
        {
            var obj1JSON = JsonSerializer.Serialize(object1);
            var obj2JSON = JsonSerializer.Serialize(object2);
            Assert.That(obj1JSON.Equals(obj2JSON));
        }

        [SetUp]
        public void SetUp()
        {
            Node mip1 = new Node(Operator.AND, "Море", "Пальмы");
            moreipalmyAST = new BinaryTree(mip1);

            Node mipil2 = new Node(Operator.OR, mip1, "Лес");
            moreipalmyililesAST = new BinaryTree(mipil2);

            Node cmplx = new Node(Operator.AND, mipil2, "Ягоды");
            complexAST = new BinaryTree(cmplx);
        }

        [Test]
        public void BuildAst_WhenGivenQuery_MoreIPalmy()
        {
            Interpreter interpreter = new Interpreter("Море и Пальмы");

            BinaryTree ast = interpreter.GetAST();
            EqualByJSON(ast, moreipalmyAST);
        }

        [Test]
        public void BuildAst_WhenGivenQuery_MoreIPalmyIliLes()
        {
            Interpreter interpreter = new Interpreter("Море и Пальмы или Лес");

            BinaryTree ast = interpreter.GetAST();
            EqualByJSON(ast, moreipalmyililesAST);
        }

        [Test]
        public void BuildAst_WhenGivenQuery_Complex()
        {
            Interpreter interpreter = new Interpreter("Море и Пальмы или Лес и Ягоды");

            BinaryTree ast = interpreter.GetAST();
            EqualByJSON(ast, complexAST);
        }
    }
}