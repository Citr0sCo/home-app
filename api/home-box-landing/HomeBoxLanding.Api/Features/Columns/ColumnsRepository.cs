using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.Columns;

public interface IColumnsRepository
{
    Task<List<ColumnRecord>> GetAll();
    Task<CreateColumnResponse> Create(CreateColumnRequest request);
    Task<UpdateColumnResponse> Update(UpdateColumnRequest request);
    Task<CommunicationResponse> Delete(Guid reference);
    Task<ImportColumnsResponse> Import(ImportColumnsRequest request);
}

public class ColumnsRepository : IColumnsRepository
{
    public async Task<List<ColumnRecord>> GetAll()
    {
        await using (var context = new DatabaseContext())
        {
            try
            {
                return context.Columns
                    .Include(x => x.Links)
                    .OrderBy(x => x.SortOrder)
                    .ToList();
            }
            catch (Exception exception)
            {
                return new List<ColumnRecord>();
            }
        }
    }
    
    public async Task<CreateColumnResponse> Create(CreateColumnRequest request)
    {
        var response = new CreateColumnResponse();
            
        var column = request.Column;

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var columnRecord = new ColumnRecord
                {
                    Identifier = Guid.NewGuid(),
                    Name = column.Name,
                    SortOrder = column.SortOrder,
                    Icon = column.Icon,
                    Links = new List<LinkRecord>()
                };

                context.Add(columnRecord);
                    
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Column = new Column
                {
                    Identifier = columnRecord.Identifier,
                    Name = columnRecord.Name,
                    SortOrder = columnRecord.SortOrder,
                    Icon = columnRecord.Icon,
                    Links = columnRecord.Links.ConvertAll(LinkMapper.Map)
                };
                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to create a column.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }

    public async Task<UpdateColumnResponse> Update(UpdateColumnRequest request)
    {
        var response = new UpdateColumnResponse();
            
        var column = request.Column;

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var columnRecord = context.Columns.FirstOrDefault(x => x.Identifier == request.Column.Identifier);
                    
                if (columnRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.DatabaseError,
                        UserMessage = "Something went wrong attempting to update a Column.",
                        TechnicalMessage = "Something went wrong attempting to update a Column."
                    });
                    return response;
                }

                if (column.Name.Length > 0 && column.Name != columnRecord.Name)
                    columnRecord.Name = column.Name;

                if (column.Icon.Length > 0 && column.Icon != columnRecord.Icon)
                    columnRecord.Icon = column.Icon;

                context.Update(columnRecord);
                    
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Column = ColumnMapper.Map(columnRecord);
                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a Column.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }

    public async Task<CommunicationResponse> Delete(Guid reference)
    {
        var response = new CommunicationResponse();

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var columnRecord = context.Columns.FirstOrDefault(x => x.Identifier == reference);
                    
                if (columnRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.DatabaseError,
                        UserMessage = "Something went wrong attempting to delete a column.",
                        TechnicalMessage = "Something went wrong attempting to delete a column."
                    });
                    
                    return response;
                }

                context.Columns.Remove(columnRecord);
                    
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a build log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }

    public async Task<ImportColumnsResponse> Import(ImportColumnsRequest request)
    {
        var response = new ImportColumnsResponse();
            
        var columns = request.Columns;

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                foreach (var columnRecord in context.Columns)
                {
                    context.Remove(columnRecord);
                }
                
                foreach (var column in columns)
                {
                    var columnRecord = new ColumnRecord
                    {
                        Identifier = Guid.NewGuid(),
                        Name = column.Name,
                        SortOrder = column.SortOrder,
                        Icon = column.Icon
                    };

                    context.Add(columnRecord);

                    foreach (var link in column.Links)
                    {
                        context.Add(new LinkRecord
                        {
                            Identifier = Guid.NewGuid(),
                            Name = link.Name,
                            Url = link.Url,
                            Host = link.Host,
                            Port = link.Port,
                            IconUrl = link.IconUrl,
                            IsSecure = link.IsSecure,
                            SortOrder = link.SortOrder,
                            Column = columnRecord,
                        });
                    }
                }
                    
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Columns = columns;
                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a build log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }
}