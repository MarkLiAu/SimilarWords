import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class NotFound extends Component {
    displayName = NotFound.name

  render() {
    return (
      <div>
            <h2> Sorry, the page is not found</h2>
            <Link to={'/'}>Home</Link>

      </div>
    );
  }
}
