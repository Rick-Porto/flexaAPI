using flexa.App.DTOs.Screen;
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.App.Interfaces;
using AutoMapper;

namespace flexa.App.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _repository;
    private readonly IMapper _mapper;

    public ScreenService(IScreenRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ScreenResponse>> GetAllAsync(string userId)
    {
        var screens = await _repository.GetAllAsync(userId);
        return _mapper.Map<IEnumerable<ScreenResponse>>(screens);
    }

    public async Task<ScreenResponse?> GetByIdAsync(int id, string userId)
    {
        var screen = await _repository.GetByIdAsync(id, userId);
        return screen != null ? _mapper.Map<ScreenResponse>(screen) : null;
    }

    public async Task<ScreenResponse> AddAsync(ScreenRequest request, string userId)
    {
        var screen = _mapper.Map<Screen>(request);
        await _repository.AddAsync(screen, userId);
        return _mapper.Map<ScreenResponse>(screen);
    }

    public async Task UpdateAsync(int id, ScreenRequest request, string userId)
    {
        var screen = _mapper.Map<Screen>(request);
        screen.Id = id;
        await _repository.UpdateAsync(screen, userId);
    }

    public async Task DeleteAsync(int id, string userId)
    {
        await _repository.DeleteAsync(id, userId);
    }
}