import React from 'react';
import ReactDOM from 'react-dom';
import { Router, Route, browserHistory } from 'react-router';

class Shell extends React.Component {
    render() {
        return (<div>{this.props.children}</div>);
    }
}

class NotFound extends React.Component {
    render() {
        return (<h2>Path <span className="mono">{this.props.location.pathname}</span> not found</h2>);
    }
}

ReactDOM.render(
    <Router history={browserHistory}>
        <Route path="/" component={Shell} />
        <Route path="*" component={NotFound} />
    </Router>,
    document.getElementById('root')
);