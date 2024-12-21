using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityFrameworkCore.Rules;

public class PreventEfUpdateInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
    {
        if (command.CommandText.Contains("__EFMigrationsHistory"))
        {
            throw new InvalidOperationException(
                "Direct execution of 'dotnet ef database update' is not allowed. Please use the migrator project.");
        }

        return base.NonQueryExecuting(command, eventData, result);
    }
}