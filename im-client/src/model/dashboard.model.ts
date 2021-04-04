import { ChartType, DashLayout } from "@/model/enums.model";

export type DashElement = {
  queryId: string;
  chartType: ChartType;
  width: number;
  height: number;
}

export type DashPage = {
  layout: DashLayout;
  elements: DashElement[];
};