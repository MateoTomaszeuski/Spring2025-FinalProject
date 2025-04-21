import React from 'react';

export class MainPage extends React.Component {
  render() {
    return (
      <>
        {/* we can add some images to this page (when we have them) so it looks less empty */}

        <div id="main-page-readme">
          <p>A student time management app, with a social twist!</p>
          <p>Organize your courses and assignments through
            custom to-do lists, productivity insights, and additional
            study tools like a Pomodoro timer, calculator, and scratchpad for quick notes.
          </p>
          <p>
            Stay on top of assignments by sending them directly to your to-do list.
          </p>
          <p>
            See statistics tailored to your  workflow and share your
            progress with friends and peers.
          </p>
        </div>
        {/* <a id="download-button" href='https://youtu.be/dQw4w9WgXcQ?si=cxQcwxCL5z5MN--Q' target='_blank'>Download Now!</a> */}
      </>
    );
  }
}
