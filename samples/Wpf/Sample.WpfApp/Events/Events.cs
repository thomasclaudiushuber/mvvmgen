namespace Sample.WpfApp.Events
{
  record EmployeeSavedEvent(int EmployeeId,string FirstName);
  record EmployeeSelectedEvent(int EmployeeId);
}
