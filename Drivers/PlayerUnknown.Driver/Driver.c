#include <ntdef.h>
#include <ntifs.h>

DRIVER_INITIALIZE DriverEntry;
#pragma alloc_text(INIT, DriverEntry)

NTSTATUS NTAPI MmCopyVirtualMemory
(
	PEPROCESS		SourceProcess,
	PVOID			SourceAddress,
	PEPROCESS		TargetProcess,
	PVOID			TargetAddress,
	SIZE_T			BufferSize,
	KPROCESSOR_MODE PreviousMode,
	PSIZE_T			ReturnSize
);

NTKERNELAPI NTSTATUS PsLookupProcessByProcessId(
	_In_		HANDLE		ProcessId,
	_Outptr_	PEPROCESS	*Process
);

NTSTATUS DriverEntry(
	_In_		struct _DRIVER_OBJECT	*DriverObject,
	_In_		PUNICODE_STRING			RegistryPath
)
{
	return STATUS_SUCCESS;
}

NTSTATUS KeReadProcessMemory(PEPROCESS Process, PVOID SourceAddress, PVOID TargetAddress, SIZE_T Size)
{
	SIZE_T Result;

	PEPROCESS SourceProcess = Process;
	PEPROCESS TargetProcess = PsGetCurrentProcess();

	if (NT_SUCCESS(MmCopyVirtualMemory(SourceProcess, SourceAddress, TargetProcess, TargetAddress, Size, KernelMode, &Result)))
		return STATUS_SUCCESS;
	else
		return STATUS_ACCESS_DENIED;
}

NTSTATUS KeWriteProcessMemory(PEPROCESS Process, PVOID SourceAddress, PVOID TargetAddress, SIZE_T Size)
{
	SIZE_T Result;

	PEPROCESS SourceProcess = PsGetCurrentProcess();
	PEPROCESS TargetProcess = Process;

	if (NT_SUCCESS(MmCopyVirtualMemory(SourceProcess, SourceAddress, TargetProcess, TargetAddress, Size, KernelMode, &Result)))
		return STATUS_SUCCESS;
	else
		return STATUS_ACCESS_DENIED;

}

