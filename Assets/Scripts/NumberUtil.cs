public class NumberUtil {

	public static int FromZToN(int z)
	{
		// positive numbers turned to positive even number
		// negative number truned to positive odd number
		return z >= 0 ? 2 * z : -2 * z - 1;
	}

	public static int FromVectorToN(int x, int y)
	{
		x = FromZToN(x);
		y = FromZToN(y);
		var k = x + y;
		return  k * (k+1) / 2 + x;
	}
}
