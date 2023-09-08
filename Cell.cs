
namespace Sudoku_solver
{
    internal class Cell
    {
        public Cell(Button button)
        {
            Button = button;
        }

        public Button Button { get; }
        public int Value {  get; set; }
    }
}
