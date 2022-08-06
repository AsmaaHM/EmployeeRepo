import { Injectable } from '@angular/core';  
import { HttpClient, HttpHeaders } from '@angular/common/http';  

 
import { environment } from 'src/environments/environment';  
import { Employee } from '../_models/employee';

  
@Injectable({  
  providedIn: 'root'  
})  
export class EmployeeService {  

  constructor(private httpClient: HttpClient) {

  }

  private employeesUrl = environment.apiUrl + '/api/employees';  

  AddEmployee(input) 
  {
    return this.httpClient.post<Employee>(this.employeesUrl, input);
  }

  UpdateEmployee(input) 
  {
    const url = `${this.employeesUrl}/${input.id}`;  
    return this.httpClient.put<Employee>(url, input);
  }

  GetEmployees() 
  {
    return this.httpClient.get<Employee[]>(this.employeesUrl);
  }
  
  DeleteEmployee(input) 
  {
    const url = `${this.employeesUrl}/${input.id}`;  
    return this.httpClient.delete<Employee>(url, input);
  }
 
}    