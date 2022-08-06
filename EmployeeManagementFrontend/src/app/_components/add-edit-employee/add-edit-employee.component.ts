import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Employee } from 'src/app/_models/employee';
import { EmployeeService } from 'src/app/_services/employee.service';


@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
  styleUrls: ['./add-edit-employee.component.css']
})
export class AddEditEmployeeComponent implements OnInit {
  @Input() Employee: Employee;
  errorMessage: string;

  constructor(private emplyeeService: EmployeeService) { }

  ngOnInit(): void {
  }


  SubmitForm(form: NgForm) {

    if (form.invalid)
      return;

    if(this.Employee.id==0)  
    this.AddEmployee(); 
    else 
    this.UpdateEmployee();  
  }



  AddEmployee() {
    this.emplyeeService.AddEmployee(this.Employee)
    .subscribe(  
      () => this.onSaveComplete(),  
      (err) => this.errorMessage = "An error has occurred, please try again later."
    );  

  }

  UpdateEmployee() {
    this.emplyeeService.UpdateEmployee(this.Employee)
    .subscribe(  
      () => this.onSaveComplete(),  
      (err) => this.errorMessage = "An error has occurred, please try again later."
    );  

  }

  onSaveComplete(): void {
    
  }


  DisplayError() {


  }

}
