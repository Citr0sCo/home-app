using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Folders.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.Folders;

public interface IFoldersRepository
{
    Task<List<FolderRecord>> GetAll();
    Task<CreateFolderResponse> Create(CreateFolderRequest request);
    Task<UpdateFolderResponse> Update(UpdateFolderRequest request);
    Task<CommunicationResponse> Delete(Guid reference);
}

public class FoldersRepository : IFoldersRepository
{
    public async Task<List<FolderRecord>> GetAll()
    {
        await using (var context = new DatabaseContext())
        {
            try
            {
                return context.Folders
                    .Include(x => x.Links)
                    .OrderBy(x => x.SortOrder)
                    .ToList();
            }
            catch (Exception exception)
            {
                return new List<FolderRecord>();
            }
        }
    }

    public async Task<CreateFolderResponse> Create(CreateFolderRequest request)
    {
        var response = new CreateFolderResponse();

        var folder = request.Folder;

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var columnRecord = context.Columns.FirstOrDefault(x => x.Identifier == folder.ColumnId);

                if (columnRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.DatabaseError,
                        UserMessage = "Something went wrong attempting to create a folder.",
                        TechnicalMessage = $"No column exists with the identifier {folder.ColumnId}."
                    });
                    return response;
                }

                var folderRecord = new FolderRecord
                {
                    Identifier = Guid.NewGuid(),
                    Name = folder.Name,
                    Icon = folder.Icon,
                    SortOrder = folder.SortOrder,
                    Column = columnRecord
                };

                context.Add(folderRecord);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Folder = FolderMapper.Map(folderRecord);
                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to create a folder.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }

    public async Task<UpdateFolderResponse> Update(UpdateFolderRequest request)
    {
        var response = new UpdateFolderResponse();

        var folder = request.Folder;

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var folderRecord = context.Folders
                    .Include(x => x.Links)
                    .FirstOrDefault(x => x.Identifier == folder.Identifier);

                if (folderRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.DatabaseError,
                        UserMessage = "Something went wrong attempting to update a folder.",
                        TechnicalMessage = "Something went wrong attempting to update a folder."
                    });
                    return response;
                }

                if (folder.Name.Length > 0 && folder.Name != folderRecord.Name)
                    folderRecord.Name = folder.Name;

                if (folder.Icon.Length > 0 && folder.Icon != folderRecord.Icon)
                    folderRecord.Icon = folder.Icon;

                context.Update(folderRecord);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Folder = FolderMapper.Map(folderRecord);
                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a folder.",
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
                var folderRecord = context.Folders
                    .Include(x => x.Links)
                    .FirstOrDefault(x => x.Identifier == reference);

                if (folderRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.DatabaseError,
                        UserMessage = "Something went wrong attempting to delete a folder.",
                        TechnicalMessage = "Something went wrong attempting to delete a folder."
                    });

                    return response;
                }

                // Deleting a folder releases its links back into the parent column rather than destroying them.
                foreach (var linkRecord in folderRecord.Links)
                {
                    linkRecord.FolderIdentifier = null;
                    linkRecord.Folder = null;
                }

                await context.SaveChangesAsync();

                context.Folders.Remove(folderRecord);

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
                    UserMessage = "Something went wrong attempting to delete a folder.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }
}
