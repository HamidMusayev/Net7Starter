﻿using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.File;
using DTO.Responses;
using ENTITIES.Enums;
using File = ENTITIES.Entities.File;

namespace BLL.Concrete;

public class FileService : IFileService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public FileService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<IResult> AddAsync(FileToAddDto dto, FileUploadRequestDto requestDto)
    {
        var fileId = await AddAsync(dto);

        switch (dto.Type)
        {
            case FileType.UserProfile:
                await _userService.AddProfileAsync(requestDto.UserId!.Value, fileId.Data);
                break;
            case FileType.OrganizationLogo:
                // because of organization services are in mediatr
                // we don't need to inject this to here and add mediatr package to BLL just for this line.
                // that is example line and shows logic
                //await _organizationService.AddLogoAsync(requestDto.OrganizationId!.Value, fileId.Data);
                break;
        }

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> RemoveAsync(FileRemoveRequestDto dto)
    {
        await SoftDeleteAsync(dto.HashName);

        switch (dto.Type)
        {
            case FileType.UserProfile:
                await _userService.AddProfileAsync(dto.UserId!.Value, null);
                break;
            case FileType.OrganizationLogo:
                //await _organizationService.AddProfileAsync(userId!.Value, null);
                break;
        }

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<FileToListDto>> GetAsync(string hashName)
    {
        var data = await _unitOfWork.FileRepository.GetAsync(m => m.HashName == hashName);
        if (data is null) return new ErrorDataResult<FileToListDto>(Messages.DataNotFound.Translate());

        var mapped = _mapper.Map<FileToListDto>(data);

        return new SuccessDataResult<FileToListDto>(mapped, Messages.Success.Translate());
    }

    private async Task<IDataResult<int>> AddAsync(FileToAddDto dto)
    {
        var data = _mapper.Map<File>(dto);

        var added = await _unitOfWork.FileRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessDataResult<int>(added.Id, Messages.Success.Translate());
    }

    private async Task<IResult> SoftDeleteAsync(string hashName)
    {
        var data = await _unitOfWork.FileRepository.GetAsync(m => m.HashName == hashName);

        _unitOfWork.FileRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}