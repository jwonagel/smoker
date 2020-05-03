import { Component, OnInit } from '@angular/core';
import { SmokerService, MeasurementClient } from '../services/api';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  constructor(private smokerService: SmokerService) { }

  values: string[];
  latestMeasurement: MeasurementClient;


  ngOnInit(): void {
    this.smokerService.smokerLatestGet()
      .subscribe(e => this.latestMeasurement = e);
  }

}
