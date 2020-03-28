using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace ShoppingCartHandlers.DataAccess.TypeHandlers
{
    public class MessageNumberEventHandler : SqlMapper.TypeHandler<MessageNumber>
    {
        public override void SetValue(IDbDataParameter parameter, MessageNumber value)
        {
            parameter.Value = value == MessageNumber.NotSet ? -1 : value.Value;
        }

        public override MessageNumber Parse(object value)
        {
            return (int) value == -1 ? MessageNumber.NotSet : MessageNumber.New((int)value);
        }
    }
}
