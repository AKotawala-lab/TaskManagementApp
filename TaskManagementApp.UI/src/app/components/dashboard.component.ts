import { Component, OnInit } from '@angular/core';
import { TaskService } from '../services/task.service';
import { AuthenticationService } from '../services/authentication.service';
import { Task } from '../models/task.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  user: any;
  currentDate: Date = new Date();
  highPriorityTasks: Task[] = [];
  mediumPriorityTasks: Task[] = [];
  lowPriorityTasks: Task[] = [];
  errorMessage: string = '';

  constructor(private taskService: TaskService, private authService: AuthenticationService) { }

  ngOnInit(): void {
    this.user = this.authService.getCurrentUser();
    this.fetchTasks();
  }

  fetchTasks(): void {
    const dateStr = this.currentDate.toISOString().split('T')[0];
    const token = this.authService.getToken();
    if (token) {
      this.taskService.getTasksForDate(dateStr, token).subscribe({
        next: (tasks) => {
          this.highPriorityTasks = tasks.filter(task => task.priority === 'high');
          this.mediumPriorityTasks = tasks.filter(task => task.priority === 'medium');
          this.lowPriorityTasks = tasks.filter(task => task.priority === 'low');
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

  previousDay(): void {
    this.currentDate.setDate(this.currentDate.getDate() - 1);
    this.fetchTasks();
  }

  nextDay(): void {
    this.currentDate.setDate(this.currentDate.getDate() + 1);
    this.fetchTasks();
  }

  toggleDetails(task: Task): void {
    task.showDetails = !task.showDetails;
  }

  editTask(task: Task): void {
    // Implement task editing logic here
  }

  deleteTask(task: Task): void {
    // Implement task deletion logic here
  }
}
