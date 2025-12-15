using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFlow
{
	public  class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("hey Enter Your First Number");
		
			int a = Convert.ToInt32(Console.ReadLine());

			Console.WriteLine("Hey Enter Your Second Number");

			int b = Convert.ToInt32(Console.ReadLine());
			int c = a + b;

			Console.WriteLine("result" + c);
			Console.ReadLine();

			



		}
	}
}
