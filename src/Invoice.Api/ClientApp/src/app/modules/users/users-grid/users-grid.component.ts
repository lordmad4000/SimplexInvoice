import { Location } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UserResponse } from 'src/app/shared/models/userresponse';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-users-grid',
  templateUrl: './users-grid.component.html',
  styleUrls: ['./users-grid.component.css']
})
export class UsersGridComponent implements OnInit {

  displayedColumns: string[] = [
    "email",
    "firstname",
    "lastname"
  ];

  start: number = 0;
  limit: number = 50;
  end: number = this.limit + this.start;
  selectedRowIndex: number = 0;

  users: UserResponse[] = [];
  dataSource = new MatTableDataSource<UserResponse>();

    constructor(private userservice: UserService,
                private router: Router,
                private location: Location) {
  }

  ngOnInit(): void {
    this.loadUsersData();
  }

  loadUsersData() {
    this.userservice.GetAll().subscribe({
      next: (res: any) => {
        const data = res;
        if (data) {
          this.users = data;
          this.dataSource = new MatTableDataSource(this.getTableData(this.start, this.end));
          this.updateIndex();
        }
      },
      error: (err : HttpErrorResponse) => {
        console.log('Error al recuperar los usuarios', err);
      }
    });
  }

  getRecord(row: UserResponse) {
    console.log(row);
    this.router.navigate(['/users/view', `${row.id}`]);
  }

  onTableScroll(event: any) {
    const tableViewHeight = event.target.offsetHeight;
    const tableScrollHeight = event.target.scrollHeight;
    const scrollLocation = event.target.scrollTop;

    const buffer = 200;
    const limit = tableScrollHeight - tableViewHeight - buffer;
    if (scrollLocation > limit) {
      let data = this.getTableData(this.start, this.end);
      this.dataSource.data = this.dataSource.data.concat(data);
      this.updateIndex();
    }
  }

  getTableData(start: number, end: number) {
    return this.users.slice(start, end);
  }

  updateIndex() {
    this.start = this.end;
    this.end = this.limit + this.start;
  }

    addButtonClick(event: any) {
        console.log('Add button.');
        this.router.navigate(['/users/new']);
    }

    backButtonClick(event: any) {
        console.log('Back button.');
        this.location.back();
    }
}