using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace YGOPRO_CardManager
{
	public class Program
	{

        public static Dictionary<int, CardInfo> CardData = new Dictionary<int, CardInfo>();
		[STAThread]
		static void Main ()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run (new Main_frm());
			
		}
	}
}

