using static Utils.Enumeratori;

namespace Utils
{
    public class Filter
    {
        // Nome del campo da filtrare
        public string Key { get; set; }
        // Valore che deve essere filtrato
        public string Value { get; set; }

        // Il valore del filtro è una query
        public string SqlValue { get; set; } = "";

        // Valore mm el cas che l fl debba cadee  u ae (BEWEEN)
        public string MinValue { get; set; }
        // Valore massm el cas che l fl debba cadee  u ae (BEWEEN)
        public string MaxValue { get; set; }

        /// <summary>
        /// Tipo del campo da filtrare
        /// Valori ammessi: "Ukwnown", "String", "Number", "Dacimale", "Scientific", "DateYear", "DateMonth"
        /// </summary>
        public FilterDataType Type { get; set; } = FilterDataType.Unknown;

        //Indica l'operatore da usare per il filtro.
        // Valori ammessi: "=" , "<" , ">", "BETWEEN", "LIKE" 
        public string Operator { get; set; }
    }
}