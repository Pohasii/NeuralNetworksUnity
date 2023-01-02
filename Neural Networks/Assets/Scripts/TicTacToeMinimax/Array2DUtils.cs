public static class Array2DUtils
{
    public static int ToIndex(int row, int indexInRow, int width)
    {
        return row * width + indexInRow;
    }
}
