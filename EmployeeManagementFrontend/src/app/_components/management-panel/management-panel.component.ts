import { Component, OnInit } from '@angular/core';
import {Employee} from '../../_models/employee'
import {EmployeeService} from '../../_services/employee.service'
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-management-panel',
  templateUrl: './management-panel.component.html',
  styleUrls: ['./management-panel.component.css']
})
export class ManagementPanelComponent implements OnInit {
  Employees: Employee[] = [];
  errorMessage: string;
  SelectedEmployee: Employee = new Employee();
  NewEmployee: Employee = new Employee(); 
  
  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.GetEmployees();
  }


  GetEmployees() {
      this.employeeService.GetEmployees().pipe(first()).subscribe(employees => {
          this.Employees = employees;
      });
  }

  DeleteEmployee() {
    this.employeeService.DeleteEmployee(this.SelectedEmployee)
    .subscribe(  
      () => 
      (err) => this.errorMessage = "An error has occurred, please try again later."
    );  
  }

}
