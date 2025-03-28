import React from 'react';
import { PomodoroDisplay } from './PomodoroDisplay';

export class DemoPage extends React.Component<Record<string, never>, {display: DisplayOptions}> {
  constructor() {
    super({});
    this.state = {
      display: "Pomodoro"
    }
  }
  render() {
    return(
      <div>
        <PomodoroDisplay />
      </div>
    )
  }
}

type DisplayOptions = 
  | "Pomodoro";
