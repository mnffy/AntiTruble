
namespace AntiTruble.ClassLibrary.Enums
{
    public enum RepairStatuses : byte
    {
        //Заявка принята;
        Confirm = 0,
	    //Устройство поступило в СЦ;
        EquipmentInCenter = 1,
	    //Диагностика неисправности;
        Diagnostic = 2,
	    //Устройство в процессе ремонта;
        Repair = 3,
	    //Ожидается поставка комплектующих;
        WaitingParts = 4,
	    //Устройство отремонтировано;
        CompletedRepair = 5,
	    //Ожидает оплаты;
        PayWaiting = 6,
	    //Возвращено клиенту.
        BackToClient = 7,
        //Заявка отклонена
        Cancel = 8
    }
}
