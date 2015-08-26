using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GoatTrip.DAL.DTOs;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace GoatTrip.DAL
{
    public interface ILocationGroupBuilder
    {
         LocationGroup Build(IDataRecord readerDataObject, IEnumerable<SqLiteQueryField> groupByFields);
         LocationGroup Build(Document document, IEnumerable<SqLiteQueryField> groupByFields);
         string GenerateGroupDescription(IDataRecord readerDataObject, IEnumerable<SqLiteQueryField> groupByFields);
         string GenerateGroupDescription(Document document, IEnumerable<SqLiteQueryField> groupByFields);
    }
}
