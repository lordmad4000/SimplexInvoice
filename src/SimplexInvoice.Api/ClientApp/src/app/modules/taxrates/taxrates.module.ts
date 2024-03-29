import { BaseGridComponent } from 'src/app/components/base-grid';
import { CommonModule } from '@angular/common';
import { CustomTranslateService } from 'src/app/shared/services/customtranslate.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TaxRatesComponent } from './taxrates.component';
import { TaxRatesEditComponent } from './taxrates-edit';
import { TaxRatesGridComponent } from './taxrates-grid/taxrates-grid.component';
import { TaxRatesNewComponent } from './taxrates-new';
import { TaxRatesRoutingModule } from './taxrates-routing.module';
import { TaxRatesService } from 'src/app/shared/services/taxrates.service';
import { TaxRatesViewComponent } from './taxrates-view';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    TaxRatesComponent,
    TaxRatesGridComponent,
    TaxRatesEditComponent,
    TaxRatesNewComponent,
    TaxRatesViewComponent
  ],
  imports: [
    CommonModule,
    TaxRatesRoutingModule,
    MatTableModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatDividerModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatToolbarModule,
    MatSnackBarModule,
    MatSortModule,
    BaseGridComponent,
    TranslateModule
  ],
  providers: [ 
    TaxRatesService,
    CustomTranslateService
  ],
})
export class TaxRatesModule { }