import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { formatDate } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TaskService } from '../services/task.service';
import { AuthenticationService } from '../services/authentication.service';
import { AddEditTaskComponent } from './task/add-edit-task.component';
import { Task, TaskPriority } from '../models/task.model';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  user: any;
  currentDate: Date = new Date();
  allTasks: Task[] = [];
  highPriorityTasks: Task[] = [];
  mediumPriorityTasks: Task[] = [];
  lowPriorityTasks: Task[] = [];
  errorMessage: string = '';
  searchTerm: string = '';
  avatarUrl: string = 'https://upload.wikimedia.org/wikipedia/commons/a/a2/Person_Image_Placeholder.png';
  sortDirection: { [key: string]: boolean } = {
    high: true,
    medium: true,
    low: true
  };

  constructor(
    private router: Router,
    private taskService: TaskService, 
    private authService: AuthenticationService,
    private modalService: NgbModal) { }

  ngOnInit(): void {
    
    this.user = this.authService.getCurrentUser();
    if (this.user.avatarUrl)
      this.avatarUrl = this.user.avatarUrl;
    this.fetchTasks();
  }

  fetchTasks(): void {
    const token = this.authService.getToken();
    if (token) {
      this.taskService.getAllTasks(this.user.id, token).subscribe({
        next: (tasks) => {
          this.allTasks = tasks;
          this.filterTasks();
        },
        error: (err) => {
          this.errorMessage = 'Failed to load tasks';
          console.error(err);
        }
      });
    } else {
      this.errorMessage = 'Authentication token not found';
    }
  }

  filterTasks(): void {
    const dateStr = this.currentDate.toISOString().split('T')[0];
    if (this.searchTerm) {
      this.highPriorityTasks = this.allTasks.filter(task => 
        task.dueDate.split('T')[0] === dateStr &&
        task.priority === TaskPriority.High &&
        task.title.toLowerCase().includes(this.searchTerm.toLowerCase()));
      this.mediumPriorityTasks = this.allTasks.filter(task => 
        task.dueDate.split('T')[0] === dateStr &&
        task.priority === TaskPriority.Medium &&
        task.title.toLowerCase().includes(this.searchTerm.toLowerCase()));
      this.lowPriorityTasks = this.allTasks.filter(task => 
        task.dueDate.split('T')[0] === dateStr &&
        task.priority === TaskPriority.Low &&
        task.title.toLowerCase().includes(this.searchTerm.toLowerCase()));
    } else {
      this.highPriorityTasks = this.allTasks.filter(task => 
        task.dueDate.split('T')[0] === dateStr &&
        task.priority === TaskPriority.High);
      this.mediumPriorityTasks = this.allTasks.filter(task => 
        task.dueDate.split('T')[0] === dateStr &&
        task.priority === TaskPriority.Medium);
      this.lowPriorityTasks = this.allTasks.filter(task => 
        task.dueDate.split('T')[0] === dateStr &&
        task.priority === TaskPriority.Low);
    }
  }

  toggleSort(priority: string): void {
    this.sortDirection[priority] = !this.sortDirection[priority];
    switch (priority) {
      case 'high':
        this.highPriorityTasks.sort((a, b) =>
          this.sortDirection[priority]
            ? new Date(a.dueDate).getTime() - new Date(b.dueDate).getTime()
            : new Date(b.dueDate).getTime() - new Date(a.dueDate).getTime()
        );
        break;
      case 'medium':
        this.mediumPriorityTasks.sort((a, b) =>
          this.sortDirection[priority]
            ? new Date(a.dueDate).getTime() - new Date(b.dueDate).getTime()
            : new Date(b.dueDate).getTime() - new Date(a.dueDate).getTime()
        );
        break;
      case 'low':
        this.lowPriorityTasks.sort((a, b) =>
          this.sortDirection[priority]
            ? new Date(a.dueDate).getTime() - new Date(b.dueDate).getTime()
            : new Date(b.dueDate).getTime() - new Date(a.dueDate).getTime()
        );
        break;
    }
  }

  previousDay(): void {
    const newDate = new Date(this.currentDate);
    newDate.setDate(this.currentDate.getDate() - 1);
    this.currentDate = newDate;
    this.filterTasks();
  }

  nextDay(): void {
    const newDate = new Date(this.currentDate);
    newDate.setDate(this.currentDate.getDate() + 1);
    this.currentDate = newDate;
    this.filterTasks();
  }

  toggleDetails(task: Task): void {
    task.showDetails = !task.showDetails;
  }

  editTask(task: Task): void {
    const modalRef = this.modalService.open(AddEditTaskComponent);
    modalRef.componentInstance.task = task;

    modalRef.result.then((result) => {
      if (result) {
        this.fetchTasks();
      }
    }).catch((error) => {
      console.log('Modal dismissed', error);
    });
  }

  deleteTask(task: Task): void {
    const confirmation = window.confirm('Are you sure you want to delete this task?');
    if (confirmation) {  
      const token = this.authService.getToken();
      this.taskService.deleteTask(task.id, token).subscribe({
        next: () => {
          this.fetchTasks();
        },
        error: (error) => {
          this.errorMessage = 'Failed to delete task. Please try again.';
          console.error('Delete task error:', error);
        }
      });
    }
  }

  openCreateTaskModal(): void {
    const modalRef = this.modalService.open(AddEditTaskComponent);
    modalRef.componentInstance.task = null;

    modalRef.result.then((result) => {
      if (result) {
        this.fetchTasks();
      }
    }).catch((error) => {
      console.log('Modal dismissed', error);
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
