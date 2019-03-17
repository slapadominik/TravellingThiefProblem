using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using TTP.Entities;

namespace TTP
{
    public class FileHelper : IFileHelper
    {
        public List<string> ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            return File.ReadAllLines(path).ToList();
        }

        public bool IsFileDataValid(IEnumerable<string> fileLines)
        {
            return fileLines == null ||
                   fileLines.Count(x => x.StartsWith(Consts.ProblemName) ||
                                       x.StartsWith(Consts.KnapsackDataType) ||
                                       x.StartsWith(Consts.Dimension) ||
                                       x.StartsWith(Consts.KnapsackCapacity) ||
                                       x.StartsWith(Consts.MinSpeed) ||
                                       x.StartsWith(Consts.MaxSpeed) ||
                                       x.StartsWith(Consts.NodeCoordSection) ||
                                       x.StartsWith(Consts.ItemsSection)) == 8;
        }

        public TTPData ParseFileData(List<string> fileData)
        {
            TTPData data = new TTPData();
            data.ProblemName = fileData[0].Split('\t')[1];
            data.KnapsackDataType = fileData[1].Split(':')[1];
            data.Dimension = int.Parse(fileData[2].Split('\t')[1]);
            data.KnapsackCapacity = int.Parse(fileData.ElementAt(4).Split('\t')[1]);
            data.MinSpeed = double.Parse(fileData.ElementAt(5).Split('\t')[1], CultureInfo.InvariantCulture);
            data.MaxSpeed = double.Parse(fileData.ElementAt(6).Split('\t')[1], CultureInfo.InvariantCulture);
            data.RentingRatio = double.Parse(fileData.ElementAt(7).Split('\t')[1], CultureInfo.InvariantCulture);

            var nodeCoordIndex = fileData.FindIndex(x => x.StartsWith(Consts.NodeCoordSection));
            var itemsSectionIndex = fileData.FindIndex(x => x.StartsWith(Consts.ItemsSection));

            for (int i = nodeCoordIndex + 1; i < itemsSectionIndex; i++)
            {
                City city = new City();
                city.Index = int.Parse(fileData[i].Split('\t')[0]);
                city.X = (int)double.Parse(fileData[i].Split('\t')[1], CultureInfo.InvariantCulture);
                city.Y = (int)double.Parse(fileData[i].Split('\t')[2], CultureInfo.InvariantCulture);
                data.Cities.Add(city);
            }
            for (int i = itemsSectionIndex + 1; i < fileData.Count; i++)
            {
                Item item = new Item();
                item.Index = int.Parse(fileData[i].Split('\t')[0]);
                item.Profit = int.Parse(fileData[i].Split('\t')[1]);
                item.Weight = int.Parse(fileData[i].Split('\t')[2]);
                item.AssignedNode = int.Parse(fileData[i].Split('\t')[3]);
                data.Items.Add(item);
            }
            return data;
        }

        public void WriteToCsv<T>(IEnumerable<T> records, string path)
        {
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<TSPStatisticsCsvModelMapper>();
                csv.WriteRecords(records);
            }
        }
    }
}