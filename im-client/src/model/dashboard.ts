import { ChartType } from "@/model/enums";

export type DashElement = {
  elementId: number;
  chartType: ChartType;
  width: number;
  height: number;
}

export type DashPages = DashElement[][];