using AutoMapper;
using Educational.Core.DAL;

namespace Educational.Core.BLL.Services.Abstract;

public abstract class BaseService
{
    private protected readonly DataContext _db;
    private protected readonly IMapper _mapper;

    public BaseService(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
}
