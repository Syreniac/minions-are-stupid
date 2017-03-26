using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

//public class TerrainWriter : MonoBehaviour {
public class TerrainWriter {

	private const string path = "./TerrainFiles/";

	/*private int chunksX;
	private int chunksY;
	private int chunkWidth;
	private int chunkHeight;*/

	private string header;
	private List<string> lines;

	public TerrainWriter() {
		lines = new List<string>();
	}

	public void StoreHeader(int chunksX, int chunksY, int chunkWidth, int chunkHeight) {
		/*this.chunksX = chunksX;
		this.chunksY = chunksY;
		this.chunkWidth = chunkWidth;
		this.chunkHeight = chunkHeight;*/
		
		header = "# " + chunksX + ":" + chunksY + ":" + chunkWidth + ":" + chunkHeight;
	}

	public void StoreGrid(HexCell[,] cells) {

		StringBuilder sb = new StringBuilder();

		for(int x = 0; x < cells.GetLength(0); x++) {
			for(int y = 0; y < cells.GetLength(1); y++) {
				sb.Append(cells[x,y].getHeight() + ",");
			}
			lines.Add(sb.ToString());
			/*sb.Clear();*/
			sb = new StringBuilder();
		}
	}

	public void WriteToFile(string filename = "TerrainFile") {
		using(StreamWriter sr = new StreamWriter(path + filename)) {
			sr.WriteLine(header);
			foreach(string line in lines) {
				sr.WriteLine(line);
			}
		}
	}
}

/*public class TerrainReader {

	private const string path = "./TerrainFiles/";

	private int chunksX;
	private int chunksY;
	private int chunkWidth;
	private int chunkHeight;

	private string header;
	private List<string> lines;

	public TerrainWriter() {
		lines = new List<string>;
	}








	public void StoreHeader(int chunksX, int chunksY, int chunkWidth, int chunkHeight) {
		this.chunksX = chunksX;
		this.chunksY = chunksY;
		this.chunkWidth = chunkWidth;
		this.chunkHeight = chunkHeight;
		
		header = "# " + chunksX + ":" + chunksY + ":" + chunkWidth + ":" + chunkHeight;
	}

	public void StoreGrid(HexCell[,] cells) {

		StingBuilder sb = new StringBuilder;

		for(int x = 0; x < cells.GetLength(0); x++) {
			for(int y = 0; y < cells.GetLength(1); y++) {
				sb.Append(cells[x,y].getHeight() + ",");
			}
			lines.Append(sb.ToString());
			sb.Clear();
		}
	}

	public void WriteToFile(string filename = "TerrainFile") {
		using(StreamWriter sr = new StreamWriter(path + filename)) {
			foreach(string line in lines) {
				sr.WriteLine(line);
			}
		}
	}
}*/
